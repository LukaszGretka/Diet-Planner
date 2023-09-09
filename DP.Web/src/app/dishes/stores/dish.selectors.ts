import { createFeatureSelector } from '@ngrx/store';
import { DishState } from './dish.state';

const getState = createFeatureSelector<DishState>('dishState');
