import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectThemeArray } from '../../stores/groupstore.selector';

@Component({
  selector: 'app-theme',
  templateUrl: './theme.component.html',
  styleUrl: './theme.component.css'
})
export class ThemeComponent {
  theme$ = this.store.select(selectThemeArray)
  constructor(private store: Store) { }
}
