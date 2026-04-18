import React from "react";

type BaseModalProps = {
  id: string;
  title: React.ReactNode;
  show: boolean;
  onClose: () => void;
  children: React.ReactNode;
  footer?: React.ReactNode;
  size?: "sm" | "lg" | "xl";
};

export function BaseModal({
                            id,
                            title,
                            show,
                            onClose,
                            children,
                            footer,
                            size,
                          }: BaseModalProps) {
  return (
    <div className={`modal fade ${show ? "show d-block" : ""}`} id={id}>
      <div className={`modal-dialog ${size ? `modal-${size}` : ""}`}>
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
  );
}