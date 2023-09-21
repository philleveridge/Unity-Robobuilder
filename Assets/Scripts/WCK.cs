using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using UnityEngine.UIElements;

public class WCK : MonoBehaviour
{
    // Start is called before the first frame update
    public string ComPort = "COM5";
    public int Baudrate = 115200;

    private SerialPort port;

    private bool isConnected = false;

    void Start()
    {
        try
        {
            port = new SerialPort(ComPort, Baudrate);
            Debug.Log("Connected to " + ComPort);
            port.ReadTimeout = 500;
            port.WriteTimeout = 500;

            port.Open();
            if (port.IsOpen)
                isConnected = true;
        }
        catch {
            Debug.Log("Failed to connect to " + ComPort);
            isConnected=false;  
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SendOperCommand(byte D1, byte D2 )
    {
        byte cs = (byte)((D1 ^ D2) & 0x7F);
        port.Write(new byte[] { 0xff, D1, D2, cs }, 0, 4);
    }

    byte[] readWord() 
    {
        byte[] buffer = new byte[2];
        port.Read(buffer, 0, 2);
        return buffer;
    }

    public byte[] setservo(int id, int pos)
    {
        if (isConnected)
        {
            // do something

            byte torque=4, position=(byte)(pos&0xFF);
            byte sid = (byte)((torque<<5) | (byte)(id&0xFF)) ;
            Debug.Log("SET " + id + "=" + position);
            SendOperCommand(sid, position);
            return readWord();
        }
        Debug.Log("SET: Port Not connected?");
        return new byte[] { 0xFF, 0xFF };
    }

    public byte[] getservo(int ServoID)
    {
        if (isConnected)
        {
            Debug.Log("GET " + ServoID);
            byte sid = (byte)(0xA0 | (byte)(ServoID&0xFF));
            SendOperCommand(sid, 0);
            return readWord();
        }
        Debug.Log("GET: Port Not connected?");
        return new byte[] { 0xFF, 0xFF };
    }

}
