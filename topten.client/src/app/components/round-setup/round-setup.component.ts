import { Component, Input } from '@angular/core';
import { SignalrService } from '../../services/signalr.service';
import { Store } from '@ngrx/store';
import { GroupModel } from '../../interfaces/groupmodel.model';
import { selectRemainingHp, selectRoundNumber } from '../../stores/groupstore.selector';

@Component({
  selector: 'app-round-setup',
  templateUrl: './round-setup.component.html',
  styleUrl: './round-setup.component.css'
})
export class RoundSetupComponent {
  roundNumber$ = this.store.select(selectRoundNumber)

  @Input() info: string = ''

  constructor(private store: Store<GroupModel>) { }
}
