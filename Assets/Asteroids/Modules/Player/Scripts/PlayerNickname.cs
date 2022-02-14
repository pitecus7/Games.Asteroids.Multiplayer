using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNickname : NetworkBehaviour
{
    [SerializeField] private SettingsChannel settinsChannel;

    [SerializeField] private TextMesh nicknameTextMesh;

    [SerializeField] private bool hideText;

    [SyncVar(hook = nameof(NicknameChanged)), SerializeField] private string nickname;
    public string Nickname => nickname;

    private float alphaCurrentValue;

    private Transform following;

    [SerializeField, Range(0.0f, 1.0f)]
    private float interested = 2f; //how interested the follower is in the thing it's following

    [SerializeField, Range(-10.0f, 10.0f)]
    private float offset = -1f; //Y offset from following(Parent)

    private void NicknameChanged(string oldValue, string newValue)
    {
        ShowNickname(newValue);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
            CMDInitValues(settinsChannel.nicknamePlayer);
    }

    private void Start()
    {
        nicknameTextMesh = GetComponent<TextMesh>();
        alphaCurrentValue = nicknameTextMesh.color.a;
        //Remove from parent, it want to be free!
        following = transform.parent;
        transform.SetParent(null, true);
    }

    [Command]
    public void CMDInitValues(string _nickname)
    {
        nickname = _nickname;
    }

    void Update()
    {
        if (hideText)
        {
            nicknameTextMesh.color = new Color(1, 1, 1, alphaCurrentValue);
            alphaCurrentValue -= Time.deltaTime / 2;

            if (alphaCurrentValue <= 0.05f)
            {
                gameObject.SetActive(false);
                hideText = false;
            }
        }
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(following.transform.position.x, offset + following.transform.position.y, following.transform.position.z), interested);
    }

    public void ShowNickname(string nickname)
    {
        alphaCurrentValue = 1;
        nicknameTextMesh.text = nickname;
        gameObject.SetActive(true);
    }

    public void HideNickname()
    {
        hideText = true;
    }
}
