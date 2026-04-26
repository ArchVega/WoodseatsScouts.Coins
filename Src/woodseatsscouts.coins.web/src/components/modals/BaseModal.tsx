import "./BaseModal.scss"
import React from "react";

type BaseModalProps = {
  id: string;
  title: React.ReactNode;
  show: boolean;
  onClose: () => void;
  isPhotoModal?: boolean;
  children: React.ReactNode;
  footer?: React.ReactNode;
  size?: "sm" | "lg" | "xl";
};

export function BaseModal({
                            id,
                            title,
                            show,
                            onClose,
                            isPhotoModal,
                            children,
                            footer,
                            size,
                          }: BaseModalProps) {
  return (
    <>
      {show && (
        <div
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            width: "100vw",
            height: "100vh",
            backgroundColor: "rgba(0, 0, 0, 0.7)", // semi-transparent black
            zIndex: 1040, // slightly below modal
          }}
          onClick={onClose} // close modal when clicking backdrop
        />
      )}
      <div className={`modal fade ${show ? "show d-block" : ""}`} id={id}>
        <div className={`modal-dialog modal-dialog-scrollable ${size ? `modal-${size}` : ""} ${isPhotoModal ? "modal-right" : ""} `}>
          <div className="modal-content">

            <div className="modal-header">
              <h5 className="modal-title">{title}</h5>
              <button type="button" className="btn-close" onClick={onClose}/>
            </div>

            <div className="modal-body">{children}</div>

            {footer && <div className="modal-footer">{footer}</div>}
          </div>
        </div>
      </div>
    </>
  );
}