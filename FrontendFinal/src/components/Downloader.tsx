import React, { useRef, ReactNode } from "react";
import HTMLtoDOCX from "@turbodocx/html-to-docx";
import { saveAs } from "file-saver";
import { Printer } from "lucide-react";

interface DocxExportWrapperProps {
  children: ReactNode;
  filename?: string;
  buttonLabel?: string;
  disabled?: boolean;
}

const DocxExportWrapper: React.FC<DocxExportWrapperProps> = ({
  children,
  filename = "document.docx",
  buttonLabel = "Download (DOCX)",
  disabled = false,
}) => {
  const contentRef = useRef<HTMLDivElement>(null);

  const handleDownload = async () => {
    if (!contentRef.current) return;

    const contentHtml = contentRef.current.innerHTML;
    const fullHtml = `<!DOCTYPE html><html><head><meta charset="utf-8"></head><body>${contentHtml}</body></html>`;

    const fileBuffer = await HTMLtoDOCX(fullHtml, null, {
      table: { row: { cantSplit: true } },
    });

    saveAs(fileBuffer as Blob, filename);
  };

  return (
    <>
      <button
        className="btn btn-primary"
        onClick={handleDownload}
        disabled={disabled}
      >
        <Printer size={16} /> {buttonLabel}
      </button>
      <div ref={contentRef}>{children}</div>
    </>
  );
};

export default DocxExportWrapper;
