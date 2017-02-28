﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Networking;
using Windows.Networking.Sockets;
using System.Diagnostics;
using Windows.Networking.Connectivity;
using Windows.Storage.Streams;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SmartCar
{
    public sealed class StartupTask : IBackgroundTask
    {
        private SmartCar car;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //
            taskInstance.Canceled += TaskInstance_Canceled;
            StartListening();
            //Prevent from exit
            taskInstance.GetDeferral();
            //MainDrivingScreen screen = new MainDrivingScreen();
            
            //screen.

        }
        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            Debug.Write(reason);
        }

        public async void StartListening()
        {
            foreach (HostName localHostInfo in NetworkInformation.GetHostNames())
            {
                if (localHostInfo.IPInformation != null)
                {
                    DatagramSocket socket = new DatagramSocket();
                    socket.MessageReceived += Sock_MessageReceived;

                    await socket.BindEndpointAsync(localHostInfo, "8888");
                }
            }
        }


        private void Sock_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            using (DataReader reader = args.GetDataReader())
            {
                string value = reader.ReadString(reader.UnconsumedBufferLength);
                switch (value.Trim())
                {
                    case "create":
                        car = new SmartCar();
                        break;
                    case "forward":
                        if (car != null)
                        {
                            car.FowardBackword(Direction.Forward);
                        }
                        break;
                    case "backward":
                        if (car != null)
                        {
                            car.FowardBackword(Direction.Backward);
                        }
                        break;
                    case "turnright":
                        if (car != null)
                        {
                            car.TurnRight();
                        }
                        break;
                    case "turnleft":
                        if (car != null)
                        {
                            car.TurnLeft();
                        }
                        break;
                    case "backright":
                        if (car != null)
                        {
                            car.TurnBackwardRight();
                        }
                        break;
                    case "backleft":
                        if (car != null)
                        {
                            car.TurnBackwardLeft();
                        }
                        break;
                    case "stop":
                        if (car != null)
                        {
                            car.Stop();
                        }
                        break;
                    case "speed":
                        if (car != null)
                        {
                            car.SpeedTest();
                        }
                        break;
                }
            }
        }



    }
}
