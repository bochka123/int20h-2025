import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { HubConnectionState } from 'redux-signalr';

const baseUrl = import.meta.env.VITE_API_URL;
const hubUrl = import.meta.env.VITE_HUB_URL;

export const useConnection = (): { connection: HubConnection | null; } => {
    const [connection, setConnection] = useState<HubConnection | null>(null);
  
    useEffect(() => {
      if (baseUrl) {
        if (!!connection) {
          connection.stop();
        }
        const newConnection = new HubConnectionBuilder()
          .withUrl(baseUrl + hubUrl, {
            transport: HttpTransportType.LongPolling,
            withCredentials: true
          })
          .withAutomaticReconnect()
          .build();
  
        setConnection(newConnection);
      }
    }, [baseUrl]);
  
    useEffect(() => {
      if (connection && connection.state !== HubConnectionState.Connected) {
        connection
          .start()
          .catch(error => {
            console.error(error);
          });
      }
    }, [connection]);
  
    return {
      connection,
    };
  };
