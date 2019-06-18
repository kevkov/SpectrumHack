import React from "react";
import {Container, Content, Text} from "native-base";
import Constants from "expo-constants";

export const Home = () => {
    return (
        <Container style={{paddingTop: Constants.statusBarHeight}}>
            <Content>
                <Text>Home screen</Text>
            </Content>
        </Container>
    )
};
