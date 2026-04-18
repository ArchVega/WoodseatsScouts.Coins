export default function AppLocalStorage() {
  const keys = {
    useAppCamera: "woodseatsscouts.preferences.use-camera"
  }

  return {
    getUseAppCamera: () => {
      return localStorage.getItem(keys.useAppCamera) === "true"
    },
    setUseAppCamera: (use: boolean) => {
      localStorage.setItem(keys.useAppCamera, String(use));
    }
  }
}