import getAppSettings from "../../AppSettings.ts";

export function usePasscode() {
  const appSettings = getAppSettings();

  const checkPasscode = () => {
    const input = prompt('Enter 4-digit passcode:');

    if (!input || input.trim() === '') {
      return false;
    } else if (input === appSettings.VITE_ADMIN_PASSCODE) {
      return true;
    } else {
      alert('Incorrect passcode!');
      return false;
    }
  };

  return {checkPasscode};
}