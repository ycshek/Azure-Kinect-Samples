using BestHTTP.SocketIO3;
using Microsoft.Azure.Kinect.BodyTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SkeletonDataHandler : MonoBehaviour
{
    public string SkeletonID = "Sender";// Sender or Receiver
    public main m;
    public GameObject playerBody;
    public TrackerHandler KinectDevice;
    public Quaternion[] absoluteJointRotations = new Quaternion[(int)JointId.Count];
    SocketManager manager = new SocketManager(new Uri("http://localhost:3000"));
    Socket root;

    // Start is called before the first frame update
    void Start()
    {
        // Accessing the root ("/") socket
        root = manager.Socket;

        // add listener to socket if it is receiver
        if (SkeletonID == "Receiver")
            root.On<string, string, bool>("ReceiveSkeletonPoint", (arg1, arg2, arg3) => SkeletonPointStringToQuaternion(arg1, arg2, arg3));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // send skeleton data to socket if it is sender
        if (SkeletonID == "Sender")
            SkeletonPointQuaternionToString();

        //SkeletonPointStringToQuaternion(skeletonPoints);
        //Debug.Log(skeletonPoints);
    }

    void SkeletonPointQuaternionToString()
    {
        string skeletonPoints = "";
        for (int i = 0; i < KinectDevice.absoluteJointRotations.Length; i++)
        {
            skeletonPoints += KinectDevice.absoluteJointRotations[i].ToString("F8") + ";";
        }

        if (root != null && m != null)
        {
            root.Emit("SendSkeletonPoint", SkeletonID, skeletonPoints, m.playerActive);
        }
    }    

    void SkeletonPointStringToQuaternion(string skeletonID, string skeletonPoints, bool active)
    {
        if (active)
        {
            string[] skeletonPointString = skeletonPoints.Split(';');

            for (int i = 0; i < (int)JointId.Count; i++)
            {
                absoluteJointRotations[i] = QuaternionParse(skeletonPointString[i]);
            }

            if (playerBody != null)
                playerBody.SetActive(true);
        }
        else if (playerBody != null)
        {
            playerBody.SetActive(false);
        }
    }

    Quaternion QuaternionParse(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Quaternion.identity;

        name = name.Replace("(", "").Replace(")", "");
        string[] s = name.Split(',');
        //Debug.Log(string.Join(",",s));
        return new Quaternion(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
    }
}
