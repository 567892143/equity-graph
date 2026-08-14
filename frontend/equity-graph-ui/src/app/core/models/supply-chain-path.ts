export interface ChainNode {
  id: string;
  name: string;
}

export interface SupplyChainPath {
  nodes: ChainNode[];
  dependencyPercentages: number[];
  hops: number;
}
