interface ImportMetaEnv {
    readonly VITE_REACT_APP_URL: string;
    readonly VITE_CALCULATOR_API_URL: string;
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}