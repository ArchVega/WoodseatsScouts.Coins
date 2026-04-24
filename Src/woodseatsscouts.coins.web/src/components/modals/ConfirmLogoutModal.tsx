import {BaseModal} from "./BaseModal.tsx";
import {Button} from "../widgets/HtmlControlWrappers.tsx";

export default function ConfirmLogoutModal({showConfirmLogoutModal, setShowConfirmLogoutModal}) {
  return (
    <BaseModal id={"app-settings-modal"} title={"Log out?"} show={showConfirmLogoutModal} onClose={() => {
      setShowConfirmLogoutModal(false)
    }}>
      <div className="row mb-3">
        <div className="col">
          <p className="fs-2 text-center">Any tokens scanned will won't be saved! Are you sure?</p>
        </div>
      </div>
      <div className="row start-over-modal-buttons">
        <div className="col">
          <Button className="btn btn-danger" onClick={() => window.location.reload()} data-testid="button-confirm-start-again">
            Yes, log me out
          </Button>
        </div>
        <div className="col">
          <Button type="button" className="btn btn-success float-end" onClick={() => setShowConfirmLogoutModal(false)}>
            No, leave me in
          </Button>
        </div>
      </div>
    </BaseModal>
  )
}