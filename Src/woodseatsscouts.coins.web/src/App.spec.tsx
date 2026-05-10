import {expect, test, vi} from "vitest";
import App from "@/App.tsx";
import {render, screen} from "@testing-library/react";
import * as storageModule from "@/components/storage/AppLocalStorage.ts";

vi.spyOn(storageModule, 'default').mockReturnValue({
  getAppSettings: vi.fn().mockReturnValue(false),
} as any)

test('renders teh correct text', () => {
  render(<App/>);
  const textboxElement = screen.getByTestId("textbox-usb-scanner-code")
  expect(textboxElement).toBeInTheDocument()
})