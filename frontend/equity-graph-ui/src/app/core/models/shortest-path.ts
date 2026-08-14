export interface PathNode {
  id: string;
  name: string;
  label: string;
}

export interface ShortestPathResponse {
  nodes: PathNode[];
  relationshipTypes: string[];
  hops: number;
}
