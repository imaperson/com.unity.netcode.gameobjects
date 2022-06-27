using System;
using System.Collections;
using UnityEngine;

namespace Unity.Netcode
{
    [RequireComponent(typeof(NetworkSimulator))]
    public class NetworkScenario : MonoBehaviour
    {
        NetworkSimulator m_NetworkSimulator;

        [SerializeReference]
        internal INetworkSimulatorScenario m_NetworkSimulatorScenario;

        public INetworkSimulatorScenario NetworkSimulatorScenario
        {
            get => m_NetworkSimulatorScenario;
            set => m_NetworkSimulatorScenario = value;
        }

        NetworkSimulator NetworkSimulatorSimulator => m_NetworkSimulator == null
            ? m_NetworkSimulator = GetComponent<NetworkSimulator>()
            : m_NetworkSimulator;

        IEnumerator Start()
        {
            if (NetworkSimulatorSimulator == null)
            {
                throw new ArgumentNullException($"{nameof(NetworkSimulatorSimulator)} cannot be null.");
            }

            if (NetworkSimulatorScenario == null)
            {
                Debug.LogWarning($"You need to select a valid {nameof(NetworkScenario)}.");
                yield break;
            }

            while (NetworkSimulatorSimulator.IsInitialized == false)
            {
                yield return null;
            }
            
            NetworkSimulatorScenario.Start(NetworkSimulatorSimulator.NetworkEventsApi);
        }
        
        void OnDestroy()
        {
            NetworkSimulatorScenario?.Dispose();
        }
        
        void Update()
        {
            if (NetworkSimulatorScenario is INetworkSimulatorScenarioUpdateHandler updatableSimulator)
            {
                updatableSimulator.Update(Time.deltaTime);
            }
        }
    }
}
