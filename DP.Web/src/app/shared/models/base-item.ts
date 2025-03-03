export class BaseItem {
  id: number;
  name: string;
  description: string;
  imagePath: string;
  itemType: ItemType;
}

export enum ItemType {
  Product,
  Dish
}
