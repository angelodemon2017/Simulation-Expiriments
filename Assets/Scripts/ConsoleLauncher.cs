using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ConsoleLauncher : MonoBehaviour
{
    private Process consoleProcess;

    private void Awake()
    {
        StartConsoleApp();
    }

    private void StartConsoleApp()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = //@"C:\Path\To\YourUnityProject\Assets\Scripts\Bin\MyConsoleApp.exe"; // Путь до вашего консольного приложения
            @"D:\Repositories\SimulationAI\Assets\Simulator\ConsoleSimulation.exe";
//        startInfo.UseShellExecute = false;

        consoleProcess = Process.Start(startInfo);
        UnityEngine.Debug.Log("Консольное приложение запущено.");
    }

    private void OnApplicationQuit()
    {
        if (consoleProcess != null && !consoleProcess.HasExited)
        {
            consoleProcess.Kill(); // Убедитесь, что процесс завершается при закрытии Unity
        }
    }
}