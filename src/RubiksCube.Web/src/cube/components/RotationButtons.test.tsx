import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { RotationButtons } from "./RotationButtons";

describe("RotationButtons", () => {
  it("renders all rotation moves as buttons", () => {
    const onMove = vi.fn();
    render(<RotationButtons onMove={onMove} />);
    expect(screen.getByRole("button", { name: "F'" })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "R" })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "D'" })).toBeInTheDocument();
  });

  it("calls onMove with the move when a button is clicked", async () => {
    const user = userEvent.setup();
    const onMove = vi.fn();
    render(<RotationButtons onMove={onMove} />);
    await user.click(screen.getByRole("button", { name: "F'" }));
    expect(onMove).toHaveBeenCalledWith("F'");
  });
});
