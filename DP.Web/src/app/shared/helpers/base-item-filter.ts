import { combineLatest, map, Observable, startWith } from 'rxjs';
import { BaseItem } from '../models/base-item';

/**
 * Filters a list of items based on a search text.
 *
 * @template T - A type that extends the `BaseItem` interface.
 * @param baseItems$ - An observable emitting an array of items to be filtered.
 * @param searchText - An observable emitting the search text used for filtering.
 * @returns An observable emitting the filtered array of items. If the search text is empty,
 *          the original array of items is returned. Items are filtered by checking if the
 *          search text is included in the `name` or `description` properties (case-insensitive).
 */
export function filterItems<T extends BaseItem>(
  baseItems$: Observable<T[]>,
  searchText: Observable<string>,
): Observable<T[]> {
  return combineLatest([baseItems$, searchText.pipe(startWith(''))]).pipe(
    map(([items, searchText]) => {
      const term = searchText.toLowerCase().trim();
      if (!term) {
        return items;
      }
      return items.filter(
        item => item.name.toLowerCase().includes(term) || item.description?.toLowerCase().includes(term),
      );
    }),
  );
}
