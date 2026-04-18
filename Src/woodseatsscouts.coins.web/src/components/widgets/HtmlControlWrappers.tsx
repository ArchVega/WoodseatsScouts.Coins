import React from "react";

/* Button ------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
type ButtonProps = React.ComponentPropsWithoutRef<"button">;

export function Button({children, className, ...props}: ButtonProps) {
  return <button {...props} className={`btn ${className ?? ""}`}>{children}</button>;
}

/* Image -------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
type ImageProps = React.ComponentPropsWithoutRef<"img">;

export function Image({children, className, style, ...props}: ImageProps) {
  const baseStyle: React.CSSProperties = {
    objectFit: "contain"
  };

  return <img {...props} className={`${className ?? ""}`} style={{...baseStyle, ...style}} alt={""}>{children}</img>;
}

/* Switch ------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
type SwitchProps = Omit<React.ComponentPropsWithoutRef<"input">, "type" | "onChange"> & {
  label?: React.ReactNode;
  checked: boolean;
  onChange: (checked: boolean) => void;
  id: string;
};

export function Switch({label, checked, onChange, id, className, ...props}: SwitchProps) {
  return (
    <div className={`form-check form-switch ${className ?? ""}`}>
      <input {...props} id={id} type="checkbox" className="form-check-input" checked={checked} onChange={(e: any) => onChange(e.target.checked)}/>

      {label && (
        <label className="form-check-label" htmlFor={id}>
          {label}
        </label>
      )}
    </div>
  );
}