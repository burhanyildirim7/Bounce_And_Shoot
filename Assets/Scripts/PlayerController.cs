using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;
    public bool isGodMode = false;
    public GameObject weapon;
    public bool isRun,isShootingTime;
    public float playerSpeed = 10;
     public Transform targetPoint1, targetPoint2, firstMovePoint, secondMovePoint,cameraTarget1,cameraTarget2;
    private int shootNo = 0;
    public int movementNo = 1;
    public Animator PlayerAnimator;
    public int bulletCount = 3;
    public int enemyCount;
    public GameObject onBoarding;


    private void Awake()
    {
        if (instance == null) instance = this;
        //else Destroy(this);
    }

    void Start()
    {
        weapon.GetComponent<LineRenderer>().enabled = false;
        StartingEvents();
        
    }

	private void FixedUpdate()
	{
        if (isRun) 
        {
            if(movementNo == 1)
            transform.position = Vector3.MoveTowards(
                  transform.position, new Vector3(firstMovePoint.position.x, transform.position.y, firstMovePoint.position.z), playerSpeed);
            else if (movementNo == 2)
                transform.position = Vector3.MoveTowards(
                      transform.position, new Vector3(secondMovePoint.position.x, transform.position.y, secondMovePoint.position.z), playerSpeed);
        }
       
    }

    public void IncreaseMovementNo()
	{
      
        enemyCount--;
        bulletCount++;
        if ( enemyCount == 0)
		{
            WinEvents();
            return;
		}
		else if (movementNo != 2)
		{
            StartCoroutine(AfterShooting());
		}
        //weapon.SetActive(false);
        if (movementNo != 2) weapon.GetComponent<LineRenderer>().enabled = false; 
        if(movementNo != 2)isShootingTime = false;

        movementNo = 2;
    }

    public void IncreaseBulletCount()
	{

	}

    IEnumerator AfterShooting()
	{
        yield return new WaitForSeconds(2.5f);

        UIController.instance.SetBulletImages();
        CameraMovement.instance.MoveCameraToTarget2();
        PlayerRunAnim();
        isRun = true;
    }

	/// <summary>
	/// Playerin collider olaylari.. collectible, engel veya finish noktasi icin. Burasi artirilabilir.
	/// elmas icin veya baska herhangi etkilesimler icin tag ekleyerek kontrol dongusune eklenir.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("collectible"))
        {
            // COLLECTIBLE CARPINCA YAPILACAKLAR...
            GameController.instance.SetScore(collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku

        }
        else if (other.CompareTag("engel"))
        {
            // ENGELELRE CARPINCA YAPILACAKLAR....
            GameController.instance.SetScore(-collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
            if (GameController.instance.score < 0) // SKOR SIFIRIN ALTINA DUSTUYSE
			{
                // FAİL EVENTLERİ BURAYA YAZILACAK..
                GameController.instance.isContinue = false; // çarptığı anda oyuncunun yerinde durması ilerlememesi için
                UIController.instance.ActivateLooseScreen(); // Bu fonksiyon direk çağrılada bilir veya herhangi bir effect veya animasyon bitiminde de çağrılabilir..
                // oyuncu fail durumunda bu fonksiyon çağrılacak.. 
			}
        }
        else if (other.CompareTag("finish")) 
        {
            // finishe collider eklenecek levellerde...
            // FINISH NOKTASINA GELINCE YAPILACAKLAR... Totalscore artırma, x işlemleri, efektler v.s. v.s.
            GameController.instance.isContinue = false;
            GameController.instance.ScoreCarp(7);  // Bu fonksiyon normalde x ler hesaplandıktan sonra çağrılacak. Parametre olarak x i alıyor. 
            // x değerine göre oyuncunun total scoreunu hesaplıyor.. x li olmayan oyunlarda parametre olarak 1 gönderilecek.
            UIController.instance.ActivateWinScreen(); // finish noktasına gelebildiyse her türlü win screen aktif edilecek.. ama burada değil..
            // normal de bu kodu x ler hesaplandıktan sonra çağıracağız. Ve bu kod çağrıldığında da kazanılan puanlar animasyonlu şekilde artacak..          
        }
        else if (other.CompareTag("firstmovepoint") && isRun )
		{
           
            Destroy(other.gameObject);
            shootNo = 1;
            transform.LookAt(targetPoint1,Vector3.up);
            ShootingTime();
		}
        else if (other.CompareTag("secondmovepoint") && isRun)
        {
            Destroy(other.gameObject);
            shootNo = 2;
            transform.LookAt(targetPoint2, Vector3.up);
            ShootingTime();
            
        }

    }


    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {
        movementNo = 1;
        shootNo = 1;
        bulletCount = 3;
        UIController.instance.SetBulletImages();
        isRun = false;
        isShootingTime = false;
        transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.parent.transform.position = Vector3.zero;
        GameController.instance.isContinue = false;
        GameController.instance.score = 0;
        transform.position = new Vector3(0, transform.position.y, 0);
        GetComponent<Collider>().enabled = true;
        PlayerIdleAnim();

    }


    public void WinEvents()
	{
		if (isShootingTime)
		{
            GameController.instance.ScoreCarp(bulletCount + 1);
            Projection.instance.ClearForNewScene();
            //weapon.SetActive(false);
            weapon.GetComponent<LineRenderer>().enabled = false;
            isRun = false;
            isShootingTime = false;
            PlayerWinAnim();
            UIController.instance.ActivateWinScreen();
            Projection.instance._spawnedObjects.Clear();
            CubeScaleMesh.instance.scalebleCube.transform.position = new(0, 20, 0);
        }
      
    }

    public IEnumerator CheckForLoose()
	{
        yield return new WaitForSeconds(2);
        if (bulletCount == 0 && enemyCount > 0) LooseEvents();
	}

    public void LooseEvents()
	{
        PlayerLooseAnim();
        Projection.instance.ClearForNewScene();
        //weapon.SetActive(false);
        weapon.GetComponent<LineRenderer>().enabled = false;
        isRun = false;
        isShootingTime = false;
        UIController.instance.ActivateLooseScreen();
        Destroy(GameObject.Find("Simulation"));
        Projection.instance._spawnedObjects.Clear();
        CubeScaleMesh.instance.scalebleCube.transform.position = new(0, 20, 0);
    }

    void ShootingTime()
	{
        Debug.Log(LevelController.instance.totalLevelNo);
        if (LevelController.instance.totalLevelNo < 2) onBoarding.SetActive(true);
        //weapon.SetActive(true);
        weapon.GetComponent<LineRenderer>().enabled = true;
        isRun = false;
        isShootingTime = true;
        Vector3 direction = Vector3.forward;
        if(shootNo == 1) direction = (targetPoint1.position - weapon.transform.position).normalized;
        else if(shootNo == 2) direction = (targetPoint2.position - weapon.transform.position).normalized;
        weapon.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        PlayerIdleAnim();
    }


    public IEnumerator FindPoints()
	{
        yield return new WaitForSeconds(.1f);
        targetPoint1 = GameObject.Find("TargetPoint1").transform;
        if(enemyCount >= 2)targetPoint2 = GameObject.Find("TargetPoint2").transform;
        cameraTarget1 = GameObject.Find("CameraTarget1").transform;
        if (enemyCount >= 2) cameraTarget2 = GameObject.Find("CameraTarget2").transform;
        firstMovePoint = GameObject.Find("FirstMovePoint").transform;
        secondMovePoint = GameObject.Find("SecondMovePoint").transform;
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("yansitici");
        GameObject[] yansitmayan = GameObject.FindGameObjectsWithTag("yansitmayan");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("engel");
        GameObject[] tnts = GameObject.FindGameObjectsWithTag("tnt");


		foreach (GameObject obs in obstacles)
		{
			Projection.instance.AddGhostToScene(obs.transform);
		}

		foreach (GameObject obs in tnts)
		{
			Projection.instance.AddGhostToScene(obs.transform);
		}

		foreach (GameObject obs in yansitmayan)
        {
            Projection.instance.AddGhostToScene(obs.transform);
        }

        foreach (GameObject obs in enemies)
        {          
            Projection.instance.AddGhostToScene(obs.transform);
        }

      

        GameObject duvar = GameObject.Find("Duvar");
        Projection.instance.AddGhostToScene(duvar.transform);
        CameraMovement.instance.MoveCameraToTarget1();
		//if(totalLevelNo > 1)Projection.instance.AddMeshCubeToScene();
		//if (LevelController.instance.totalLevelNo > 1) Projection.instance.LoadPhysicScene();
    }

	#region ANIMATION
	public void PlayerRunAnim()
	{
        ResetTriggers();
        PlayerAnimator.SetTrigger("run");
	}

    public void PlayerIdleAnim()
	{
        ResetTriggers();
        PlayerAnimator.SetTrigger("idle");
    }

    public void PlayerWinAnim()
	{
        ResetTriggers();
        PlayerAnimator.SetTrigger("win");
    }

    public void PlayerLooseAnim()
	{
        ResetTriggers();
        PlayerAnimator.SetTrigger("loose");
	}

    public void ResetTriggers()
	{
        PlayerAnimator.ResetTrigger("idle");
        PlayerAnimator.ResetTrigger("run");
        PlayerAnimator.ResetTrigger("loose");
        PlayerAnimator.ResetTrigger("win");
	}


    #endregion 

}
