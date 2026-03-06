import { describe, it, expect, vi, beforeEach } from "vitest";
import { renderHook, waitFor, act } from "@testing-library/react";
import { useCubeState } from "./useCubeState";

const solvedState = {
  up: Array(9).fill("W"),
  down: Array(9).fill("Y"),
  left: Array(9).fill("O"),
  right: Array(9).fill("R"),
  front: Array(9).fill("G"),
  back: Array(9).fill("B"),
};

describe("useCubeState", () => {
  beforeEach(() => {
    vi.restoreAllMocks();
  });

  it("fetches initial state on mount", async () => {
    const fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      json: () => Promise.resolve(solvedState),
    });
    vi.stubGlobal("fetch", fetchMock);

    const { result } = renderHook(() => useCubeState());

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(fetchMock).toHaveBeenCalled();
    expect(fetchMock.mock.calls[0][0]).toContain("/api/cube");
    expect(result.current.state).not.toBeNull();
    expect(result.current.state?.up).toEqual(Array(9).fill("W"));
    expect(result.current.state?.front).toEqual(Array(9).fill("G"));
  });

  it("applyMove sends POST with state and move and updates state", async () => {
    const postState = { ...solvedState, up: [...(solvedState.up as string[])] };
    (postState.up as string[])[6] = "O";
    (postState.up as string[])[7] = "O";
    (postState.up as string[])[8] = "O";

    const fetchMock = vi
      .fn()
      .mockResolvedValueOnce({
        ok: true,
        json: () => Promise.resolve(solvedState),
      })
      .mockResolvedValueOnce({
        ok: true,
        json: () => Promise.resolve(postState),
      });
    vi.stubGlobal("fetch", fetchMock);

    const { result } = renderHook(() => useCubeState());

    await waitFor(() => expect(result.current.loading).toBe(false));

    await act(() => result.current.applyMove("F"));

    await waitFor(() => expect(fetchMock).toHaveBeenCalledTimes(2));

    const postCall = fetchMock.mock.calls[1];
    expect(postCall[0]).toContain("/api/cube");
    expect(postCall[1]?.method).toBe("POST");
    const body = JSON.parse(postCall[1]?.body as string);
    expect(body.move).toBe("F");
    expect(body.state).toBeDefined();
    expect(result.current.state?.up[6]).toBe("O");
  });
});
