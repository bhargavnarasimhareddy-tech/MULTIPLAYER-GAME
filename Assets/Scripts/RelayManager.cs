using System;
using UnityEngine;
using TMPro;

using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManager : MonoBehaviour
{
    public TMP_InputField joinCodeInput;
    public TMP_Text joinCodeText;

    // HOST
    public async void HostGame()
    {
        try
        {
            // Initialize Unity Services
            await UnityServices.InitializeAsync();

            // Sign in anonymously
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance
                    .SignInAnonymouslyAsync();
            }

            // Create Relay allocation
            // 1 = one friend can join
            Allocation allocation =
                await RelayService.Instance
                    .CreateAllocationAsync(1);

            // Configure Unity Transport
            UnityTransport transport =
                NetworkManager.Singleton
                    .GetComponent<UnityTransport>();

            transport.SetRelayServerData(
                AllocationUtils.ToRelayServerData(
                    allocation,
                    "dtls"
                )
            );

            // Get Join Code
            string joinCode =
                await RelayService.Instance
                    .GetJoinCodeAsync(
                        allocation.AllocationId
                    );

            // Display Join Code
            joinCodeText.text =
                "Join Code: " + joinCode;

            // Start Host
            bool started =
                NetworkManager.Singleton.StartHost();

            if (started)
            {
                Debug.Log(
                    "HOST started successfully!"
                );

                Debug.Log(
                    "Join Code: " + joinCode
                );
            }
            else
            {
                Debug.LogError(
                    "Host could not start."
                );
            }
        }
        catch (Exception e)
        {
            Debug.LogError(
                "Host Error: " + e
            );
        }
    }


    // JOIN
    public async void JoinGame()
    {
        try
        {
            // Get Join Code from Input Field
            string joinCode =
                joinCodeInput.text.Trim();

            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogError(
                    "Please enter the Join Code."
                );

                return;
            }

            // Initialize Unity Services
            await UnityServices.InitializeAsync();

            // Sign in anonymously
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance
                    .SignInAnonymouslyAsync();
            }

            // Join Relay allocation
            JoinAllocation allocation =
                await RelayService.Instance
                    .JoinAllocationAsync(
                        joinCode
                    );

            // Configure Unity Transport
            UnityTransport transport =
                NetworkManager.Singleton
                    .GetComponent<UnityTransport>();

            transport.SetRelayServerData(
                AllocationUtils.ToRelayServerData(
                    allocation,
                    "dtls"
                )
            );

            // Start Client
            bool started =
                NetworkManager.Singleton.StartClient();

            if (started)
            {
                Debug.Log(
                    "CLIENT started successfully!"
                );
            }
            else
            {
                Debug.LogError(
                    "Client could not start."
                );
            }
        }
        catch (Exception e)
        {
            Debug.LogError(
                "Join Error: " + e
            );
        }
    }
}