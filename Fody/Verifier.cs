﻿using System;
using System.Diagnostics;
using NUnit.Framework;
using System.IO;
using Scalpel;

[Remove]
public static class Verifier
{
    public static void Verify(string assemblyPath2)
    {
        var exePath = Environment.ExpandEnvironmentVariables(@"%programfiles(x86)%\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\PEVerify.exe");

        if (!File.Exists(exePath))
        {
            exePath = Environment.ExpandEnvironmentVariables(@"%programfiles(x86)%\Microsoft SDKs\Windows\v8.0A\Bin\NETFX 4.0 Tools\PEVerify.exe");
        }
        var process = Process.Start(new ProcessStartInfo(exePath, $"\"{assemblyPath2}\" /IGNORE=0x80070002")
                                        {
                                            RedirectStandardOutput = true,
                                            UseShellExecute = false,
                                            CreateNoWindow = true
                                        });

        process.WaitForExit(10000);
        var readToEnd = process.StandardOutput.ReadToEnd().Trim();
        Assert.IsTrue(readToEnd.Contains($"All Classes and Methods in {assemblyPath2} Verified."), readToEnd);
    }
}