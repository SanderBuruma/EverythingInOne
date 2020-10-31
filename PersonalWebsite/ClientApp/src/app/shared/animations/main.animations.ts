import { animate, state, style, transition, trigger } from '@angular/animations';
import { AnimationTimers } from 'src/app/shared/enums/animation-timers.enum';

export let fadeIn = trigger(
  'fadeIn',
  [
    state('void', style ({opacity: 0})),
    state('default', style ({opacity: '*'})),
    transition('void => *', [
      animate(
        AnimationTimers.Fade
      )
    ])
  ]
);

export let fadeInOut = trigger(
  'fadeInOut',
  [
    state('void', style ({opacity: 0})),
    state('default', style ({opacity: '*'})),
    transition('void <=> *', [
      animate(
        AnimationTimers.Fade
      )
    ])
  ]
);

export let upInOut = trigger(
  'upInOut',
  [
    state('void', style ({transform: 'translateY(30px)'})),
    state('default', style ({transform: 'translateY(0px)'})),
    transition('void <=> *', [
      animate(
        AnimationTimers.MoveSomewhere
      )
    ])
  ]
);
