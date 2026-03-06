import "./FaceGrid.scss";

const FACE_COLORS: Record<string, string> = {
  W: "#ffffff",
  Y: "#f2f215",
  G: "#58d568",
  B: "#1c5ffe",
  R: "#ed3030",
  O: "#ffaf1c",
};

const TILE_SIZE = 52;
const TILE_GAP = 2;
export const FACE_GRID_SIZE = TILE_SIZE * 3 + TILE_GAP * 2;

export function FaceGrid({ cells }: { cells: string[] }) {
  return (
    <div className="face-grid">
      {cells.map((cell, index) => (
        <div
          key={index}
          className="face-grid__cell"
          style={{ backgroundColor: FACE_COLORS[cell] }}
        />
      ))}
    </div>
  );
}
