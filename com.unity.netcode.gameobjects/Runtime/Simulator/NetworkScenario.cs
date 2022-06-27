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

        public NetworkSimulator NetworkSimulatorSimulator => m_NetworkSimulator == null
            ? m_NetworkSimulator = GetComponent<NetworkSimulator>()
            : m_NetworkSimulator;

        void Start()
        {
            NetworkSimulatorScenario?.Start(NetworkSimulatorSimulator.NetworkEventsApi);
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
