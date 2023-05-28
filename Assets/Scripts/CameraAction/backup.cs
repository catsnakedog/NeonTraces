/*void FixedUpdate()
{
    camsize = Data.saveData.gameData.camsize;
    mainCameraC.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camsize, camSpeed);
    if(BGManager.BGObject != null)
    {
        BGManager.BGObject.transform.localScale = new Vector3(Data.saveData.gameData.camsize / 10f, Data.saveData.gameData.camsize / 10f, 1);
    }
    if (isShake == true)
    {
        Vector3 startPosition = mainCamera.transform.position;
        if (framecount < 10)
        {
            framecount+=1;
            ShakePower -= 0.1f;
            if (secondframe == false)   
            {
                mainCamera.transform.position = new Vector3(target.transform.position.x+xCorrection, target.transform.position.y+yCorrection, mainCamera.transform.position.z);
                secondframe = true;
            }
            else
            {
                mainCamera.transform.position = startPosition + Random.onUnitSphere * ShakePower;
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                    mainCamera.transform.position.y, -10f);
                secondframe = false;
            }
        }
        else
        {
            isShake = false;
            framecount = 0;
            ShakePower = 0.3f;
        }
    }
    else
    {
        
    }
}*/