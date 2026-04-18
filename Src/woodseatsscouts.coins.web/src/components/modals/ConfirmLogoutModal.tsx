import {BaseModal} from "./BaseModal.tsx";
import {Button} from "../common/HtmlControlWrappers.tsx";

export default function ConfirmLogoutModal({showConfirmLogoutModal, setShowConfirmLogoutModal}) {
  return (
    <BaseModal id={"app-settings-modal"} title={"Application Settings"} show={showConfirmLogoutModal} onClose={() => {
      setShowConfirmLogoutModal(false)
    }}>
      <div className="row mb-3">
        <div className="col">
          <p>Are you sure?</p>
        </div>
      </div>
      <div className="row start-over-modal-buttons">
        <div className="col">
          <Button className="btn btn-danger" onClick={() => window.location.reload()} data-testid="button-confirm-start-again">
            Yes
          </Button>
        </div>
        <div className="col">
          <Button type="button" className="btn btn-success" onClick={() => setShowConfirmLogoutModal(false)}>
            No
          </Button>
        </div>
      </div>
    </BaseModal>
  )
}