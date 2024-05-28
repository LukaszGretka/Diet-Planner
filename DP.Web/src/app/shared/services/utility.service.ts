import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class UtilityService {

  public getLast7Days(todayAsText: boolean = false): string[] {
    const days: string[] = [];
    const today = new Date();

    let limit = todayAsText ? 1 : 0

    for (let i = 6; i >= limit; i--) {
      const pastDate = new Date(today);
      pastDate.setDate(today.getDate() - i);

      const day = pastDate.getDate().toString().padStart(2, '0');
      const month = (pastDate.getMonth() + 1).toString().padStart(2, '0');

      days.push(`${day}.${month}`);
    }
    if (todayAsText === true) {
      days.push('today');
    }

    return days;
  }
}
