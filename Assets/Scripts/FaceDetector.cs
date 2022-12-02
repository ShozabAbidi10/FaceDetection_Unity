using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class FaceDetector : MonoBehaviour
{
    WebCamTexture _webCamTexture;
    CascadeClassifier cascade;
    //CascadeClassifier eyes;
    OpenCvSharp.Rect MyFace;
    //OpenCvSharp.Rect[] Myeyes;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        _webCamTexture = new WebCamTexture(devices[0].name);
        _webCamTexture.Play();

        // pre-trained classifier by openCV. putting this file in the asset.   
        cascade = new CascadeClassifier(Application.dataPath + @"/haarcascade_frontalface_default.xml");
        //eyes = new CascadeClassifier(Application.dataPath + @"/haarcascade_eye.xml");
    }

    // Update is called once per frame
    void Update()
    {
        // Projecting the camera frame on the plan as texture.
        //GetComponent<Renderer>().material.mainTexture = _webCamTexture;
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);

        findNewFace(frame);
        display(frame);
     }

    void findNewFace(Mat frame)
    {
        var faces = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);
        //var Eyes = eyes.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);

        if (faces.Length >= 1) // && Eyes.Length >= 1)
        {
            //Debug.Log(faces[0].Location);
            MyFace = faces[0];
            //Myeyes = Eyes;

        }
    }

    void display(Mat frame)
    {
        if(MyFace != null) // && Myeyes != null)
        {
            frame.Rectangle(MyFace, new Scalar(0, 0, 255), 3);
/*            foreach (var i in Myeyes)
            {
                frame.Rectangle(i, new Scalar(0, 255, 0), 2);
            }*/
        }

        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }


}
