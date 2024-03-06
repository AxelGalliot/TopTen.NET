import { Component } from '@angular/core';
import { AnimationOptions } from 'ngx-lottie';

@Component({
  selector: 'app-jauge',
  templateUrl: './jauge.component.html',
  styleUrl: './jauge.component.css'
})
export class JaugeComponent {
  options: AnimationOptions = {
    path: '../../assets/jauge.json'
  }

  animationItem: AnimationItem;

  updateAnimation() {
    let aData = this.animationItem['animationData']
    aData.layers[0].ks.r.k[7].s = 
  }
}
