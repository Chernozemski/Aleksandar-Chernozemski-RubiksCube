import { useState, useEffect, useCallback } from "react";
import type { CubeState, RotationMove } from "./types";

const API_URL = import.meta.env.VITE_API_URL;

export function useCubeState() {
  const [state, setState] = useState<CubeState | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchInitial = useCallback(async (isRefetch = false) => {
    if (!isRefetch) setLoading(true);
    setError(null);
    try {
      const res = await fetch(`${API_URL}/api/cube`);
      if (!res.ok) throw new Error(`GET failed: ${res.status}`);
      const data = (await res.json()) as CubeState;
      setState(data);
    } catch (e) {
      const message = e instanceof Error ? e.message : "Failed to load cube";
      setError(
        message === "Failed to fetch"
          ? "Failed to connect to the server"
          : message
      );
      setState(null);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchInitial();
  }, [fetchInitial]);

  const applyMove = useCallback(
    async (move: RotationMove) => {
      if (!state) return;
      setError(null);
      try {
        const res = await fetch(`${API_URL}/api/cube`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            state,
            move,
          }),
        });
        if (!res.ok) throw new Error(`POST failed: ${res.status}`);
        const data = (await res.json()) as CubeState;
        setState(data);
      } catch (e) {
        const message = e instanceof Error ? e.message : "Failed to apply move";
        setError(
          message === "Failed to fetch"
            ? "Failed to connect to the server"
            : message
        );
      }
    },
    [state]
  );

  return {
    state,
    loading,
    error,
    applyMove,
    refetch: useCallback(() => fetchInitial(true), [fetchInitial]),
  };
}
