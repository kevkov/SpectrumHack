import {useEffect, useState} from "react";
import {Animated} from "react-native";

export const useSlideInOutAnimation = (show: boolean, shownValue: number, hiddenValue: number) => {
    const [visible, setVisible] = useState(false);
    let showingValue = new Animated.Value(shownValue);
    let hidingValue = new Animated.Value(hiddenValue);
    let animatedValue = (visible ? showingValue : hidingValue);

    useEffect(() => {
        if (show == true && visible === false) {
            Animated.timing(
                hidingValue,
                {
                    toValue: shownValue,
                    duration: 500,
                }
            ).start(() => setVisible(true));
        } else if (visible === true && show === false) {
            Animated.timing(
                showingValue,
                {
                    toValue: hiddenValue,
                    duration: 500,
                }
            ).start(() => setVisible(false));
        }
    });
    return animatedValue;
};
