import { union } from "@ngrx/store";

const actions = union({
    
});

export type DishActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
