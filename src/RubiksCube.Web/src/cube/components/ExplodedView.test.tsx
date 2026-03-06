import { describe, it, expect } from "vitest";
import { render } from "@testing-library/react";
import { ExplodedView } from "./ExplodedView";

const state = {
  up: Array(9).fill("W"),
  down: Array(9).fill("Y"),
  left: Array(9).fill("O"),
  right: Array(9).fill("R"),
  front: Array(9).fill("G"),
  back: Array(9).fill("B"),
};

describe("ExplodedView", () => {
  it("renders three rows with six faces and placeholders for alignment", () => {
    const { container } = render(<ExplodedView state={state} />);
    const view = container.querySelector(".exploded-view");
    expect(view).toBeInTheDocument();
    const rows = view?.querySelectorAll(".exploded-row");
    expect(rows).toHaveLength(3);
    const placeholders = view?.querySelectorAll(".exploded-view__placeholder");
    expect(placeholders).toHaveLength(6);
    const faceGrids = view?.querySelectorAll(".face-grid");
    expect(faceGrids).toHaveLength(6);
    faceGrids?.forEach((grid) => expect(grid.children.length).toBe(9));
  });

  it("renders middle row with four faces (left, front, right, back)", () => {
    const { container } = render(<ExplodedView state={state} />);
    const rows = container.querySelectorAll(".exploded-row");
    expect(rows).toHaveLength(3);
    const middleRow = rows[1];
    const placeholdersInMiddle = middleRow.querySelectorAll(
      ".exploded-view__placeholder"
    );
    expect(placeholdersInMiddle).toHaveLength(0);
    const faceGridsInMiddle = middleRow.querySelectorAll(".face-grid");
    expect(faceGridsInMiddle).toHaveLength(4);
    faceGridsInMiddle.forEach((grid) => expect(grid.children.length).toBe(9));
  });

  it("returns null when state is null", () => {
    const { container } = render(<ExplodedView state={null} />);
    expect(container.firstChild).toBeNull();
  });
});
