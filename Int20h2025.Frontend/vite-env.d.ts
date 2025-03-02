interface ImportMetaEnv {
    readonly VITE_API_URL: string;
    readonly VITE_GOOGLE_CLIENT_ID: string;
    readonly VITE_AZURE_CLIENT_ID: string;
    readonly VITE_AZURE_AUTHORITY: string;
    readonly VITE_AZURE_SCOPES: string;
    readonly VITE_APPLICATION_ID_URI: string;
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}