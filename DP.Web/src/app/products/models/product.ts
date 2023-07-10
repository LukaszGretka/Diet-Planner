export class Product {
  id: number;
  name: string;
  description: string;
  imagePath: string;
  carbohydrates: number;
  proteins: number;
  fats: number;
  calories: number;
  barCode: number;
}

export interface PortionProduct extends Product {
  portionMultiplier: number;
}
