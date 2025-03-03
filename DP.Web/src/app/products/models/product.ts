import { BaseItem } from '../../shared/models/base-item';

export class Product extends BaseItem {
  carbohydrates: number;
  proteins: number;
  fats: number;
  calories: number;
  barCode: number;
}
