export interface Point {
  id: string;
  x: number;
  y: number;
  radius: number;
  color: string;
}

export interface Comment {
  id: string;
  text: string;
  backgroundColor: string;
  pointId: string;
}
