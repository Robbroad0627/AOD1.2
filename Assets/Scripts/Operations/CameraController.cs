/****************************************************************************************
 * Copyright: Bonehead Games
 * Script: CameraController.cs
 * Date Created: 
 * Created By: Rob Broad
 * Description:
 * **************************************************************************************
 * Modified By: Jeff Moreau
 * Date Last Modified: August 26, 2024
 * TODO: Variables should NEVER be public
 * Known Bugs: 
 ****************************************************************************************/

using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    //VARIABLES
    #region Inspector/Exposed Variables

    // Do NOT rename SerializeField Variables or Inspector exposed Variables
    // unless you know what you are changing
    // You will have to reenter all values in the inspector to ALL Objects that
    // reference this script.
    [SerializeField] private Transform target = null;
    [SerializeField] private Tilemap theMap = null;

    #endregion
    #region Private Variables

    private float mCameraWidth;
    private float mCameraHeight;
    private Vector3 mTopRightLimit;
    private Vector3 mBottomLeftLimit;

    #endregion

    //FUNCTIONS
    #region Initialization Functions/Methods

#pragma warning disable IDE0051
    private void Start ()
    {
        PlayerController thePlayerController = PlayerController.Access;

        if (thePlayerController != null)
        {
            target = thePlayerController.transform;
            mCameraHeight = Camera.main.orthographicSize;
            mCameraWidth = mCameraHeight * Camera.main.aspect;

            theMap.CompressBounds();
            mBottomLeftLimit = theMap.localBounds.min + new Vector3(mCameraWidth, mCameraHeight, 0f);
            mTopRightLimit = theMap.localBounds.max + new Vector3(-mCameraWidth, -mCameraHeight, 0f);

            thePlayerController.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
        }
        else
        {
            Debug.LogWarning("Camera can't find player this is normal on the main menu.");
        }
	}
#pragma warning restore IDE0051

    #endregion
    #region Implementation functions/Methods

#pragma warning disable IDE0051
    private void LateUpdate ()
    {
        Vector3 vector3 = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = vector3;

        //keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mBottomLeftLimit.x, mTopRightLimit.x), Mathf.Clamp(transform.position.y, mBottomLeftLimit.y, mTopRightLimit.y), transform.position.z);
	}
#pragma warning restore IDE0051

    #endregion
}