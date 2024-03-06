import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { GroupModel } from '../../interfaces/groupmodel.model';
import { selectFilledHpList } from '../../stores/groupstore.selector';

@Component({
  selector: 'app-hp-bar',
  templateUrl: './hp-bar.component.html',
  styleUrl: './hp-bar.component.css'
})
export class HpBarComponent {
  hpList$ = this.store.select(selectFilledHpList)

  constructor(public store: Store<GroupModel>) { }
}
