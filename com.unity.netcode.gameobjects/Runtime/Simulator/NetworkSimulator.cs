using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Unity.Netcode
{
    [RequireComponent(typeof(UnityTransport))]
    public class NetworkSimulator : MonoBehaviour, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [SerializeField, HideInInspector]
        internal Object m_ConfigurationObject;

        [SerializeReference, HideInInspector]
        internal INetworkSimulatorConfiguration m_ConfigurationReference;
        
        INetworkEventsApi m_NetworkEventsApi;

        internal INetworkEventsApi NetworkEventsApi => m_NetworkEventsApi ??= new NoOpNetworkEventsApi();
        
        internal bool IsInitialized { get; private set; }

        public INetworkSimulatorConfiguration SimulatorConfiguration
        {
            get => m_ConfigurationReference ?? m_ConfigurationObject as INetworkSimulatorConfiguration;
            set
            {
                if (value is Object networkTypeConfigurationObject)
                {
                    m_ConfigurationObject = networkTypeConfigurationObject;
                }
                else
                {
                    m_ConfigurationReference = value;
                }
                
                UpdateLiveParameters();
                OnPropertyChanged();
            }
        }

        public void UpdateLiveParameters()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            var transport = NetworkManager.Singleton.NetworkConfig?.NetworkTransport as UnityTransport;
            if (transport != null)
            {
                transport.UpdateSimulationPipelineParameters(SimulatorConfiguration);
            }
        }

        void Start()
        {
            var unityTransport = GetComponent<UnityTransport>();
            m_NetworkEventsApi = new NetworkEventsApi(this, unityTransport);
            IsInitialized = true;
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
