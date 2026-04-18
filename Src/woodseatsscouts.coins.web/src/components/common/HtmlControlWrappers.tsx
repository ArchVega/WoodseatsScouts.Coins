import React from "react";

type ButtonProps = React.ComponentPropsWithoutRef<"button">;

export function Button({children, className, ...props}: ButtonProps) {
  return <button {...props} className={`btn ${className ?? ""}`}>{children}</button>;
}

type ImageProps = React.ComponentPropsWithoutRef<"img">;

export function Image({children, className, style, ...props}: ImageProps) {
  const baseStyle: React.CSSProperties = {
    objectFit: "contain"
  };

  return <img {...props} className={`${className ?? ""}`} style={{...baseStyle, ...style}} alt={""}>{children}</img>;
}