using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pannal : EnemyBase
{
    /// <summary>
    /// �г� �� ���
    /// </summary>
    Transform pannalMain;
    Transform pillarR;
    Transform pillarL;

    /// <summary>
    /// ������ �������� �г��� ��
    /// </summary>
    Color pannelMColor = new Color(1, 0, 0, 0.2f);
    Color pannelPColor = new Color(0, 0, 1, 0.2f);

    /// <summary>
    /// ���� �гΰ� ����� �� ��ȭ�� ���� MeshRenderer
    /// </summary>
    MeshRenderer pannalMainMesh;
    MeshRenderer pillarRMesh;
    MeshRenderer pillarLMesh;

    /// <summary>
    /// �÷��̾� ���� �ּ� �ִ�ġ
    /// </summary>
    public int minDecreasePlayerBot = 1;
    public int maxDecreasePlayerBot = 3;

    /// <summary>
    /// �÷��̾� ���� �ּ� �ִ�ġ
    /// </summary>
    public int minIncreasePlayerBot = 1;
    public int maxIncreasePlayerBot = 2;

    /// <summary>
    /// ����ġ, ���߿� �÷��̾��ϰ� �ε�ġ�� �̰Ÿ�ŭ �ߵ�
    /// </summary>
    public int creaseRate;

    /// <summary>
    /// �гο� ǥ�õ� �ý�Ʈ �޽� ����
    /// </summary>
    TextMeshPro pannalText;

    /*
    /// <summary>
    /// �÷��̾� �޴��� ��ũ��Ʈ
    /// </summary>
    PlayerManager playerManager;

    /// <summary>
    /// �������� ���� ����.....
    /// </summary>
    Ybot[] ybots;
    */

    protected override void Awake()
    {
        base.Awake();
        pannalText = transform.GetChild(0).GetComponent<TextMeshPro>();
        pannalMain = transform.GetChild(1);
        pillarR = transform.GetChild(2);
        pillarL = transform.GetChild(3);

        pannalMainMesh = pannalMain.GetComponent<MeshRenderer>();
        pillarRMesh = pillarR.GetComponent<MeshRenderer>();
        pillarLMesh = pillarL.GetComponent<MeshRenderer>();

        /*
        playerManager = GameManager.instance.GetPlayerManager();
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // ���� �г� ���� �� ��ġ ����
        if(Random.value < 0.5f)
        {
            pannalMainMesh.material.color = pannelMColor;
            pillarRMesh.material.color = Color.red;
            pillarLMesh.material.color = Color.red;

            creaseRate = -(Random.Range(minDecreasePlayerBot, maxDecreasePlayerBot + 1));

        }
        else
        {
            pannalMainMesh.material.color = pannelPColor;
            pillarRMesh.material.color = Color.blue;
            pillarLMesh.material.color = Color.blue;

            creaseRate = Random.Range(minIncreasePlayerBot, maxIncreasePlayerBot + 1);
        }
        PannalText(creaseRate);

        StartCoroutine(DisableTimer(1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.back);
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if(collision.gameObject.CompareTag("PlayerManager"))
        {
            if(creaseRate > 0)
            {
                if (playerManager.YBotArrayLength() != 9)
                {
                    for (int i = 0; i < creaseRate; i++)
                    {
                        YBotPool.instance.SetActiveObject(new Vector3(0, 0.69f, 0));
                    }
                }
            }
            else
            {

            }
        }
        */
    }

    /// <summary>
    /// ������ ��Ÿ���� �ؽ�Ʈ ����
    /// </summary>
    /// <param name="crease"></param>
    void PannalText(int crease)
    {
        if(crease > 0)
        {
            pannalText.text = $"+{crease:d}";
        }
        else
        {
            pannalText.text = $"{crease:d}";
        }
        
    }

    protected override IEnumerator ActiveTime(float aliveTime = 0)
    {
        yield return new WaitForSeconds(aliveTime);

        PannalPool.instance.ObjDisable(gameObject);
    }

}
