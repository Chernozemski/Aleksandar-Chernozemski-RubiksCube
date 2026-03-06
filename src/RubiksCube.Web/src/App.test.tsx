import { describe, it, expect, vi, beforeEach } from "vitest";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import App from "./App";

const solvedState = {
  up: Array(9).fill("W"),
  down: Array(9).fill("Y"),
  left: Array(9).fill("O"),
  right: Array(9).fill("R"),
  front: Array(9).fill("G"),
  back: Array(9).fill("B"),
};

describe("App", () => {
  beforeEach(() => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue({
        ok: true,
        json: () => Promise.resolve(solvedState),
      })
    );
  });

  it("shows the title Rubik's Cube Rotator", async () => {
    render(<App />);
    expect(
      screen.getByRole("heading", { name: /rubik's cube rotator/i })
    ).toBeInTheDocument();
    await waitFor(() =>
      expect(screen.getByRole("button", { name: "F" })).toBeInTheDocument()
    );
  });

  it("shows exploded view when loaded", async () => {
    render(<App />);
    await waitFor(() =>
      expect(screen.getByRole("button", { name: "F" })).toBeInTheDocument()
    );
    expect(screen.getByRole("button", { name: "F'" })).toBeInTheDocument();
  });

  it("applies move when rotation button is clicked", async () => {
    const user = userEvent.setup();
    render(<App />);
    await waitFor(() =>
      expect(screen.getByRole("button", { name: "F" })).toBeInTheDocument()
    );
    await user.click(screen.getByRole("button", { name: "F" }));
    await waitFor(() => expect(fetch).toHaveBeenCalledTimes(2));
  });

  it("shows Reset button when loaded", async () => {
    render(<App />);
    await waitFor(() =>
      expect(screen.getByRole("button", { name: "F" })).toBeInTheDocument()
    );
    expect(screen.getByRole("button", { name: "Reset" })).toBeInTheDocument();
  });

  it("refetches cube when Reset button is clicked", async () => {
    const user = userEvent.setup();
    const fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      json: () => Promise.resolve(solvedState),
    });
    vi.stubGlobal("fetch", fetchMock);

    render(<App />);
    await waitFor(() =>
      expect(screen.getByRole("button", { name: "Reset" })).toBeInTheDocument()
    );
    expect(fetchMock).toHaveBeenCalledTimes(1);

    await user.click(screen.getByRole("button", { name: "Reset" }));
    await waitFor(() => expect(fetchMock).toHaveBeenCalledTimes(2));
    expect(fetchMock.mock.calls[1][0]).toContain("/api/cube");
    expect(fetchMock.mock.calls[1][1]).toBeUndefined();
  });
});
