import { Component } from '@angular/core';
import { SignalrService } from '../../services/signalr.service';
import { Store } from '@ngrx/store';
import { GroupModel } from '../../interfaces/groupmodel.model';
import { selectCaptain, selectFilledPlayerList, selectHost } from '../../stores/groupstore.selector';

@Component({
  selector: 'app-player-bar',
  templateUrl: './player-bar.component.html',
  styleUrl: './player-bar.component.css'
})
export class PlayerBarComponent {
  host$ = this.store.select(selectHost)
  captain$ = this.store.select(selectCaptain)
  playerList$ = this.store.select(selectFilledPlayerList)

  constructor(public signalRService: SignalrService, private store: Store<GroupModel>) { }
}
