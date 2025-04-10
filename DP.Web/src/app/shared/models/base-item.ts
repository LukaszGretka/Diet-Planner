export class BaseItem {
  id: number;
  mealItemId: number;
  name: string;
  description: string;
  imagePath: string;
  itemType: ItemType;
}

export enum ItemType {
  Product,
  Dish,
}
